// 2 params will be provided. userCred and password
if(userCred == "test") {
	//Alerts the "username"
	alert(userCred);
	if(Launcher.data.exist("kongkongmao_customlogin_perms")) {
		//Load the perms from the existing Launcher data
		Launcher.perm.loadPerm(Launcher.data.read("kongkongmao_customlogin_perms"));
	} else {
		/*
			Usually, the custom login script will be provided with internet access. But just to be safe, create it again.
		*/
		//Request for permissions. Remember, you are in a sandboxed environment!
		Launcher.perm.create();
		//Request for internet access so that we can send data.
		Launcher.perm.request([Launcher.perm.internet]);
		//Save the credentials for use next time. We don't want to annoy the user.
		Launcher.data.save("kongkongmao_customlogin_perms", Launcher.perm.setPermPersist(true));
	}
	//Check for permission
	if(Launcher.perm.canI(Launcher.perm.internet)) {
		//Save the credentials
		Launcher.data.save("kongkongmao_customlogin_respond", Launcher.server.send("http://kongkongmao.club/mc/test123", "POST", "{\"testpayload\":\"test\"}"));
	} else {
		//Alert the user
		alert("No internet access!");
		Launcher.data.delete("kongkongmao_customlogin_perms");
	}
}
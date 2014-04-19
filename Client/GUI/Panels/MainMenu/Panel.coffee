$("#mainmenu").dialog
	closeOnEscape: false
	
	draggable: false
	resizable: false
	width: 300
	minHeight: "auto"
	
	hide:
		effect: "fadeOut",
		duration: 300
		
	buttons:
		"Connect": ->
			# Disable input and attempt connecting
			$("#mainmenu-error").slideUp(200)
			$("#mainmenu").parent().find(":input").prop "disabled", true
			engine.call "connect", $("#mainmenu-form-server").val()
		"Quit": ->
			$(this).dialog("close")
			
	close: (event, ui) ->
		engine.call "quit";
	
engine.on "Network.ConnectFailed", ->
	$("#mainmenu-error").slideDown(200)
	$("#mainmenu").parent().find(":input").prop "disabled", false
	
engine.on "MainMenu.Dispose", ->
	$("#mainmenu").dialog "destroy"
$("#mainmenu").dialog
	closeOnEscape: false
	
	draggable: false
	resizable: false
	height: 180
	
	hide:
		effect: "fadeOut",
		duration: 300
		
	buttons:
		"Connect": ->
			engine.call "connect", $("#mainmenu-form-server").val()
		"Quit": ->
			$(this).dialog("close")
			
	close: (event, ui) ->
		engine.call "quit";
	
engine.on "MainMenu.Dispose", ->
	$("#mainmenu").dialog "destroy"
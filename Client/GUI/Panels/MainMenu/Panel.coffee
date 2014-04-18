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
			# Stuff here later
		"Quit": ->
			$(this).dialog("close")
			
	afterClose: (event, ui) ->
		engine.trigger('OnQuitClicked');
	
engine.on 'MainMenu.Dispose', ->
	$("#mainmenu").dialog "destroy"
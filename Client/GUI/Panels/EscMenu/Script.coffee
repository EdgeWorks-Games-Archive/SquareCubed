$("#escmenu").dialog
	dialogClass: "ui-noclose"
	
	draggable: false
	resizable: false
	autoOpen: false
	
	width: 200
	height: 400
	
	show:
		effect: "fadeIn",
		duration: 300
	hide:
		effect: "fadeOut",
		duration: 300
		
	$("#disconnect").button().click ->
		engine.call "disconnect"
	$("#backtogame").button().click ->
		$("#escmenu").dialog("close");

	
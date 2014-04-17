$("#main-menu").dialog
	resizable: false
	height: 120
	modal: true
	dialogClass: "ui-noclose"
	hide:
		effect: "fadeOut",
		duration: 300
	buttons:
		"Connect": ->
			$(this).dialog("close")
		"Quit": ->
			$(this).dialog("close")

engine.on 'MainMenu.Dispose', ->
	$("#main-menu").dialog "destroy"
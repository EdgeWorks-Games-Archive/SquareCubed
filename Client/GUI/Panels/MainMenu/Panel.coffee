parent = $("#mainmenu").parent()
$("#mainmenu").dialog
	dialogClass: "ui-noclose"
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
			engine.call "quit"
			
	open: ->
		$("#mainmenu-background").show()
	close: ->
		$("#mainmenu-background").hide()
$("#mainmenu").parent().appendTo(parent)

engine.on "MainMenu.Hide", ->
	$("#mainmenu").dialog("close")
	
engine.on "Network.ConnectFailed", ->
	$("#mainmenu-error").slideDown(200)
	$("#mainmenu").parent().find(":input").prop "disabled", false
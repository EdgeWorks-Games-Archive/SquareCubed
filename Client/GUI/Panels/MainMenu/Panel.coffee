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
			# Disable input and hide the buttons
			dialog = $("#mainmenu").parent()
			$("#mainmenu-error").slideUp 200
			dialog.find(":input").prop "disabled", true
			dialog.find(".ui-dialog-buttonpane").slideUp 200
			
			# Actually connect
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
	# Show an error message
	$("#mainmenu-error").slideDown 200
	
	# Enable input and show the buttons
	dialog = $("#mainmenu").parent()
	dialog.find(":input").prop "disabled", false
	dialog.find(".ui-dialog-buttonpane").slideDown 200
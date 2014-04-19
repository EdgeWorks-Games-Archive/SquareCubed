$("#chat").dialog
	dialogClass: "ui-noclose"
	closeOnEscape: false
	
	draggable: false
	resizable: false
	width: 400
	height: "auto"
	
	show:
		effect: "fadeIn",
		duration: 300
		
# Patch the position to something a bit more sane
$("#chat").parent().css
	position: "fixed"
	top: ""
	left: 6
	bottom: 6

$("#chat").parent().hover (
	-> $(this).fadeTo 200, 1.0) , (
	-> $(this).fadeTo 200, 0.8)
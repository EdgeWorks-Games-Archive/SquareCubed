$("#chat").dialog
	dialogClass: "ui-noclose"
	closeOnEscape: false
	
	draggable: false
	resizable: true
	
	width: 400
	minWidth: 400
	height: 216
	minHeight: 216
	
	position:
		"my": "left+6px bottom-6px"
		"at": "left bottom"
		"collision" : "none"
	
	show:
		effect: "fadeIn",
		duration: 300
		
$("#chat").parent().resizable
	handles: "n, e"

$("#chat").parent().stop(true).fadeTo 200, 0.8
$("#chat").parent().hover (
	-> $(this).stop(true).fadeTo 200, 1.0), (
	-> $(this).stop(true).fadeTo 200, 0.8)
	
# Set up the checkbox buttons
$(".chat-buttons").buttonset()

# Make enter in chat send the message
input = $("#chat-input")
input.keyup (e) ->
	if e.which is 13
		engine.call "chat.send", input.val()
		input.val ""
		
		
### Engine Events ###

chatOutput = $("#chat-output")
engine.on "chat.message", (player, message) ->
	chatLine = $("<li>", {class: "chat-message-prefix"})
	chatLine.text player + ": " + message
	chatOutput.append chatLine
	
	# Scroll to the bottom of the chat output
	chatOutput.scrollTop(chatOutput[0].scrollHeight);
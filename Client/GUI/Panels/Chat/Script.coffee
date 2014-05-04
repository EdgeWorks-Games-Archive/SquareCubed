﻿$("#chat").dialog
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
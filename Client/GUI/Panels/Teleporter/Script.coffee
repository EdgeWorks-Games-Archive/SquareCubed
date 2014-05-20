$("#teleporter").dialog
	draggable: true
	resizable: false
	autoOpen: false
	
	width: 300
	height: 200
	
	show:
		effect: "fadeIn",
		duration: 300
		
	close: ->
		engine.call "teleporter.onclose"
		
		
### Engine Events ###

engine.on "teleporter.show", ->
	$("#teleporter").dialog "open"
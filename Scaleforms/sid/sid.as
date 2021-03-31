﻿class sid extends MovieClip
{
	var globalMC : MovieClip
	
	var sidBackground : MovieClip;

	var sidLeds : Array = [10];
	
	// Constructor
	function sid(globalMovieClip)
	{
		// Invoke parent class constructor
		super();
		
		// Enable gfx extensions
		_global.gfxExtensions = true;
		
		// Save ref to global movie clip
		this.globalMC = globalMovieClip;
		
		sidBackground = this.globalMC.attachMovie("sidBackground", "sidBackground", this.globalMC.getNextHighestDepth());
		sidBackground.gotoAndStop(1);
				
		for (var column = 0;column < 10;column++) 
		{
			sidLeds[column] = Array(20);
			
			var ledX = 152 + column * 49.6;
			var ledY = 972;
			var ledYOff = 49.3;
			
			for (var i=0; i < 13;i++) 
			{
				sidLeds[column][i] = this.globalMC.attachMovie("sidLedGreen", "ledCol" + column + "Row" + i, this.globalMC.getNextHighestDepth());
			
				sidLeds[column][i].gotoAndStop(1);
				sidLeds[column][i]._x = ledX;
				sidLeds[column][i]._y = ledY - i * ledYOff;			
			}
		
			for (var i=13; i < 19;i++) 
			{
				sidLeds[column][i] = this.globalMC.attachMovie("sidLedYellow", "ledCol" + column + "Row" + i, this.globalMC.getNextHighestDepth());
			
				sidLeds[column][i].gotoAndStop(1);
				sidLeds[column][i]._x = ledX
				sidLeds[column][i]._y = ledY - i * ledYOff;
			}
		
			sidLeds[column][19] = this.globalMC.attachMovie("sidLedRed", "ledCol" + column + "Row" + i, this.globalMC.getNextHighestDepth());
			
			sidLeds[column][19].gotoAndStop(1);
			sidLeds[column][19]._x = ledX;
			sidLeds[column][19]._y = ledY - 19 * ledYOff;	
		}
	}
	
	function setLed(column, row) 
	{
		for (var index = 0;index < row;index++) 
		{
			sidLeds[column][index].gotoAndStop(2);
		}
		for (var index = row;index < 20;index++) 
		{
			sidLeds[column][index].gotoAndStop(1);
		}
	}
	
	function setBackground(_state) 
	{
		sidBackground.gotoAndStop(_state);
	}
}
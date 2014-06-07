using System;
using System.Collections.Generic;
using System.Drawing;
using OpenTK;

namespace SQCore.Client.Background
{
	internal class Space
	{
		private readonly Random _random = new Random();
		private readonly SpaceRenderer _renderer;
		private readonly Size _resolution;
		private float _fieldSize;
		private List<StarData> _starData;

		public Space(Size resolution, SpaceRenderer renderer)
		{
			// Store some metadata
			_resolution = resolution;
			_renderer = renderer;
		}

		public void GenerateStars()
		{
			// Empty/Create the star data list
			_starData = new List<StarData>();

			// Calculate the size of the star field (so we can rotate freely)
			// It's only a float and not a vector because the field will always be square
			_fieldSize = (float) Math.Sqrt(
				(_resolution.Width*_resolution.Width) +
				(_resolution.Height*_resolution.Height));

			// Calculate volume and with that amount of stars
			var volume = _fieldSize*_fieldSize;
			var starCount = volume/2500.0f; // Per 2500 volume, 1 star

			// Generate star data
			for (var i = 0; i < starCount; i++)
			{
				// Generate a new star
				_starData.Add(new StarData
				{
					Position = new Vector2(
						(float) _random.NextDouble()*_fieldSize,
						(float) _random.NextDouble()*_fieldSize),
					Rotation = (float) _random.NextDouble()*360.0f,
					Scale = (float) _random.NextDouble()*1.0f + 0.5f,
					ColorShiftDirection = (ColorShiftDirection) _random.Next(0, 4),
					ColorShiftMagnitude = (float) _random.NextDouble()*0.2f
				});
			}
		}

		public void Render()
		{
			_renderer.Render(_starData, _fieldSize, _resolution);
		}
	}

	internal enum ColorShiftDirection
	{
		White,
		White2, // < Second one to increase chance
		Red,
		Blue
	}

	internal class StarData
	{
		public Vector2 Position { get; set; }
		public float Rotation { get; set; }
		public float Scale { get; set; }
		public ColorShiftDirection ColorShiftDirection { get; set; }
		public float ColorShiftMagnitude { get; set; }
	}
}
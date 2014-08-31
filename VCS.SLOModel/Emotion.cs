using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VCS
{
	public enum Emotion
	{
		// Neutral is the default value

		Neutral,

		// Mood - emotional test

		Very_Happy,
		Happy,
		Fine,

		Sleepy,
		Bored,
		Unhappy,
		Sad,

		Tired,
		Relaxed,
		Enthusiastic,
		
		Angry,
		Anxious,
		Confused,
		Puzzled,
		Interested,
		Inspired,
		Excited,

		
		
		Other,


		// For GenevaWheel

		Relief,
		Hope,
		Interest,
		Surprise,
		Sadness,
		Fear,
		Shame,
		Guilt,
		Envy,
		Disgust,
		Contempt,
		Anger,
		Pride,
		Elation,
		Joyful,
		Satisfaction,

		None,
	}

	public enum EmotionState
	{
		Mixed,
		Enjoyment,
		Fatigue,
		Flow,
		Puzzlement,
		Relaxation,
		Sadness,
		Confussion,
		Boredom,
		Disinterest,
	}

	public static class Emotions
	{
		public static string[] GetValues()
		{
			Type t = typeof(Emotion);

			return t.GetFields().Where(x => x.IsLiteral).Select(x => x.Name).ToArray();
		}
	}
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace VoicesVideo
{
	class Program
	{
		static string Text_1_1 = "The Prototype for Live and Virtualized Collaboration is aimed at increasing the learners’ engagement and learning efficacy in collaborative activities. This is possible even when collaboration is difficult, by reusing the knowledge elicited during collaborative learning activities.";
		static string Text_1_2 = "The resulting system named Virtualized Collaborative Sessions (VCS) provides:";
		static string Text_1_3 = "Attractive and stimulating learning resources.";
		static string Text_1_4 = "Anytime anywhere reusable collaboration.";
		static string Text_1_5 = "Learning achieved through social interaction.";
		static string Text_1_6 = "Learners in control of their learning experience.";
		static string Text_1_7 = "Adhere on the standard Learning Object.";
		static string Text_1_8 = "Compatibility with different collaborative tools.";
		static string Text_2_1 = "The virtualization process is enabled by the following steps:";
		static string Text_2_2 = "1. A data source of a live collaborative learning session is considered from any web forums of online learning platforms.";
		static string Text_3 = "2. A specific converter turns the data model of a forum thread into a standardized data format CSML.";
		static string Text_4 = "3. The VCS Creator generates a Storyboard Learning Object from the thread that is stored in a repository for further reuse.";
		static string Text_5_1 = "4. The VCS Player finally reproduces the Storyboard Learning Object and allows participants to control the storyboard with different navigable options:";
		static string Text_5_2 = "Navigation within a thread.";
		static string Text_5_3 = "Navigation within a post.";
		static string Text_5_4 = "Direct access to a post.";
		static string Text_6_1 = "The ALICE technological basis IWT supports live collaborative learning by a full-featured threaded forum.";
		static string Text_6_2 = "In IWT, the participants of the discussion can create and subscribe to new threads, send posts, reply by citing others’ posts, select a scaffold for a post and vote a post.";
		static string Text_7_1 = "The ALICE project converts the text-based discussion threads of IWT forums into an animated storyboard showing how people discuss and collaborate and how discussion threads grow.";
		static string Text_7_2 = "The learner can thus leverage and reuse knowledge acquired from collaborative learning even when the live collaboration is over.";
		static string Text_8_1 = "The added values of the VCS are:";
		static string Text_8_2 = "Reusable live collaboration";
		static string Text_8_3 = "Effective virtualization of live discussions";
		static string Text_8_4 = "Emotional and affective state";
		static string Text_8_5 = "Assessment";
		static string Text_8_6 = "Content augmentation";
		static string Text_8_7 = "Meeting standards and specifications";
		static string Text_9_1 = "By the VCS, the knowledge acquired from interesting and didactic live in-class collaborative discussions is no longer wasted after its completion; instead it can be packaged for further reuse as any other learning resource.";
		static string Text_9_2 = "The added values of the VCS will greatly augment the benefits of the original live collaboration by providing learners with high quotes of challenge, empowerment, real interaction, social identity from an attractive learning resource.";
		
		static void Main(string[] args)
		{
			Speech2.Speak(Text_1_1, "Text_1_1.wav");
			Speech2.Speak(Text_1_2, "Text_1_2.wav");
			Speech2.Speak(Text_1_3, "Text_1_3.wav");
			Speech2.Speak(Text_1_4, "Text_1_4.wav");
			Speech2.Speak(Text_1_5, "Text_1_5.wav");
			Speech2.Speak(Text_1_6, "Text_1_6.wav");
			Speech2.Speak(Text_1_7, "Text_1_7.wav");
			Speech2.Speak(Text_1_8, "Text_1_8.wav");
			Speech2.Speak(Text_2_1, "Text_2_1.wav");
			Speech2.Speak(Text_2_2, "Text_2_2.wav");
			Speech2.Speak(Text_3, "Text_3.wav");
			Speech2.Speak(Text_4, "Text_4.wav");
			Speech2.Speak(Text_5_1, "Text_5_1.wav");
			Speech2.Speak(Text_5_2, "Text_5_2.wav");
			Speech2.Speak(Text_5_3, "Text_5_3.wav");
			Speech2.Speak(Text_5_4, "Text_5_4.wav");
			Speech2.Speak(Text_6_1, "Text_6_1.wav");
			Speech2.Speak(Text_6_2, "Text_6_2.wav");
			Speech2.Speak(Text_7_1, "Text_7_1.wav");
			Speech2.Speak(Text_7_2, "Text_7_2.wav");
			Speech2.Speak(Text_8_1, "Text_8_1.wav");
			Speech2.Speak(Text_8_2, "Text_8_2.wav");
			Speech2.Speak(Text_8_3, "Text_8_3.wav");
			Speech2.Speak(Text_8_4, "Text_8_4.wav");
			Speech2.Speak(Text_8_5, "Text_8_5.wav");
			Speech2.Speak(Text_8_6, "Text_8_6.wav");
			Speech2.Speak(Text_8_7, "Text_8_7.wav");
			Speech2.Speak(Text_9_1, "Text_9_1.wav");
			Speech2.Speak(Text_9_2, "Text_9_2.wav");
		}

		private static void SaveAudio(byte[] p, string p_2)
		{
			FileStream fs = new FileStream(p_2, FileMode.Create);

			fs.Write(p, 0, p.Length);

			fs.Close();
		}
	}
}

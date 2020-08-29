using System;
using System.Globalization;
using Unity;
using UnityEngine;




namespace Utility
{
    public class Utility
    {
		//Le variabili commentate sono 
		static Utility _instance;


		private float MAX_RANGE;
		private float MAX_RATE;
		private float MAX_DAMAGE;
		

		public static Utility Instance()
		{
		
				if (_instance == null)
				{
					_instance = new Utility();
				}
				return _instance;
			
		}

        public float Max_Range
        {
            get
            {
                return MAX_RANGE;
            }
        }

        public float Max_Rate
        {
            get
            {
                return MAX_RATE;
            }
        }

        public float Max_Damage
        {
            get
            {
                return MAX_DAMAGE;
            }
        }

        private Utility()
        {

            string filePath = "File/utilityFeatures";


            TextAsset data = Resources.Load<TextAsset>(filePath);
            string[] lines = data.text.Split('\n');

            for (int i = 0; i < lines.Length; i++)
            {
                string line = lines[i];
                string[] token = line.Split('=');

                switch (token[0])
                {
                    case "MAX_RANGE":
                        MAX_RANGE = float.Parse(token[1], CultureInfo.InvariantCulture);
                        break;
                    case "MAX_RATE":
                        MAX_RATE = float.Parse(token[1], CultureInfo.InvariantCulture);
                        break;
                    case "MAX_DAMAGE":
                        MAX_DAMAGE = float.Parse(token[1], CultureInfo.InvariantCulture);
                        break;
                    default:
                        
                        break;
                }
            }



        }
        }
}

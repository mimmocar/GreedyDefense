using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace DamagePackage
{
    public enum DamageType { Fire, Impact, Missile, Bullet}

    public class _Damage
    {
        private DamageType type;
        private string description;
        private float amount;

        public _Damage(DamageType type, string description, float amount)
        {
            this.type = type;
            this.description = description;
            this.amount = amount;
        }

        public string Description
        {
            get
            {
                return description;
            }

            //set
            //{
            //    this.description = value;
            //}
        }

        public DamageType Type
        {
            get
            {
                return type;
            }

            //set
            //{
            //    this.type = value;
            //}
        }

        public float Amount
        {
            get
            {
                return amount;
            }

            set
            {
                this.amount = value;
            }
        }

        public static _Damage ReadDamage(string path)
        {
            StreamReader sr = new StreamReader(path);

            DamageType type = (DamageType) Enum.Parse(typeof(DamageType),sr.ReadLine());
            string description = sr.ReadLine();
            float amount = (float)float.Parse(sr.ReadLine()); //ipotizzo formattazione del file e contenuto singolo

            return new _Damage(type, description, amount);

        }




    }
}
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace Ara.Domain.ApplicationServices
{
    [CreateAssetMenu(fileName = "EmployeeAccount", menuName = "ARA/New Employee Account", order = 0)]
    [System.Serializable]
    public class EmployeeAccount : ScriptableObject
    {
        [field: SerializeField]
        public string FirstName { get; set; }
        [field: SerializeField]
        public string LastName { get; set; }
        [field: SerializeField]
        public Texture Photo { get; set; }

    }
}
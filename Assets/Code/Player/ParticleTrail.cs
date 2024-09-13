using UnityEngine;

namespace Player
{
    public class ParticleTrail : MonoBehaviour
    {

        [SerializeField] private Rigidbody2D followRB;

        [SerializeField] private ParticleSystem effectPS;

        private bool Active;

        private void Start()
        {

            Active = false;

        }

        public void TriggleOn()
        {

            effectPS.transform.eulerAngles = new Vector3
            {
                x = 0,
                y = 0,
                z = 180 / Mathf.PI * Mathf.Atan2(-followRB.velocity.x, followRB.velocity.y)
            };

            if (Active == false)
            {

                effectPS.Play();

                Active = true;

            }

        }

        public void TriggleOff()
        {

            if(Active == true)
            {

                effectPS.Stop();

                Active = false;

            }

        }

    }
}
using System.Collections;
using UnityEngine;

namespace Platformer.Scripts
{
    internal struct CircularSawbladesProperties
    {
        public float height;
        public float angle;
        public float angleZ;
        public float depth;
    }
    
    public class BossBattle : MonoBehaviour
    {
        [Header("Arena")]
        public float arenaRadius;
        
        [Header("Sawblades")]
        public Transform sawbladesParent;
        public uint nbSawblades;
        public GameObject sawbladePrefab;

        private Transform[] _sawblades;

        public void Start()
        {
            _sawblades = new Transform[nbSawblades];
            
            for (int i = 0; i < nbSawblades; i++)
                _sawblades[i] = Instantiate(
                    sawbladePrefab, 
                    Vector3.down * 10, 
                    Quaternion.identity, 
                    sawbladesParent
                ).transform;

            StartCoroutine(BossBattleCoroutine());
        }

        private IEnumerator BossBattleCoroutine()
        {
            yield return new WaitForSeconds(1f);
            yield return BossBattlePhase1();
            yield return BossBattlePhase2();
            yield return BossBattlePhase3();
        }

        private IEnumerator BossBattlePhase1()
        {
            CircularSawbladesProperties circularSawbladesProperties1 = new CircularSawbladesProperties
            {
                height = -6,
                angle = 0,
                angleZ = 0,
                depth = 6
            };
            
            CircularSawbladesProperties circularSawbladesProperties2 = new CircularSawbladesProperties
            {
                height = 2,
                angle = 90,
                angleZ = 80,
                depth = 6
            };
            
            CircularSawbladesProperties circularSawbladesProperties3 = new CircularSawbladesProperties
            {
                height = -6,
                angle = 45,
                angleZ = 0,
                depth = 10
            };
            
            CircularSawbladesProperties circularSawbladesProperties4 = new CircularSawbladesProperties
            {
                height = 4,
                angle = 135,
                angleZ = 80,
                depth = 10
            };
            
            StartCoroutine(CircularSawblades(0, 7, circularSawbladesProperties1, circularSawbladesProperties2, 400, 4f));
            StartCoroutine(CircularSawblades(8, 15, circularSawbladesProperties3, circularSawbladesProperties4, 400, 4f));
            yield return new WaitForSeconds(5.5f);
            //StartCoroutine(CircularSawblades(0, 7, 2, 2, 90, 90, 75, 75, -12, -17, 200, 2f));
            //StartCoroutine(CircularSawblades(8, 15, 4, 4, 90 + 45, 90 + 45, 75, 75, -5, -10,200, 2f));
        }
        
        private IEnumerator BossBattlePhase2()
        {
            yield return null;
        }
        
        private IEnumerator BossBattlePhase3()
        {
            yield return null;
        }

        private IEnumerator CircularSawblades(int fromIndex, int toIndex, CircularSawbladesProperties startProperties, CircularSawbladesProperties endProperties, int steps, float duration)
        {
            float stepDuration = duration / steps;
            
            for (int step = 0; step < steps; step++)
            {
                float t = step / (float)steps;
                
                float height = Mathf.Lerp(startProperties.height, endProperties.height, t);
                float angle = Mathf.Lerp(startProperties.angle, endProperties.angle, t);
                float angleZ = Mathf.Lerp(startProperties.angleZ, endProperties.angleZ, t);
                float depth = Mathf.Lerp(startProperties.depth, endProperties.depth, t);

                for (int index = fromIndex; index <= toIndex; index++)
                {
                    SetSawbladePositionInArena(_sawblades[index], height, (index - fromIndex) / (float)(toIndex - fromIndex + 1) * 360 + angle, angleZ, depth);
                }
                
                yield return new WaitForSeconds(stepDuration);
            }
        }

        private void SetSawbladePositionInArena(Transform sawblade, float height, float angle, float angleZ, float depth)
        {
            sawblade.position = Quaternion.Euler(0, angle, 0) * Vector3.forward * depth + Vector3.up * height;
            sawblade.rotation = Quaternion.Euler(0, 180 + angle, angleZ);
        }
    }
}

// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;

// public class StatesManager : MonoBehaviour
// {
//     public GameObject fireParticle;
//     public GameObject poisonParticle;

//     private List<States> activeStates;
//     private PlayerManager playerManager;

//     Dictionary<States, string> stopStatesFunctions = new Dictionary<States, string>();

//     void Start()
//     {
//         activeStates = new List<States>();
//         playerManager = gameObject.GetComponent<PlayerManager>();
//         stopStatesFunctions.Add(States.FIRE, "fireState");
//         stopStatesFunctions.Add(States.POISON, "poisonState");
//     }

//     public void dealDamages() {
//         playerManager.TakeDamage(5);
//     }

//     private void stopFireState()
//     {
//         fireParticle.GetComponent<ParticleSystem>().Stop();
//         CancelInvoke("dealDamages");
//     }

//     private void stopPoisonState()
//     {
//         poisonParticle.GetComponent<ParticleSystem>().Stop();
//         CancelInvoke("dealDamages");
//     }

//     private void fireState() {
//         fireParticle.GetComponent<ParticleSystem>().Play();
//         Invoke("stopFireState", 3.0f);
//         InvokeRepeating("dealDamages", 0f, .75f);
//     }

//     private void poisonState()
//     {
//         poisonParticle.GetComponent<ParticleSystem>().Play();
//         Invoke("stopPoisonState", 6.0f);
//         InvokeRepeating("dealDamages", 0f, .75f);
//     }

//     public void startStates(States state) {
//         Invoke(stopStatesFunctions[state], 0f);
//     }
// }

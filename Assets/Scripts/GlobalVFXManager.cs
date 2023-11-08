using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

    public class GlobalVFXManager : SimpleSingleton<GlobalVFXManager>
    {
        [SerializeField]
        SO_VfxContainer[] _vfxContainers;

        [SerializeField]
        string path = "";

        Dictionary<string, ParticleSystem> _accessor = new Dictionary<string, ParticleSystem>();
        Dictionary<string, int> _toPool = new Dictionary<string, int>();
        private void Start()
        {
            _vfxContainers = Resources.LoadAll<SO_VfxContainer>(path);
          
            foreach (SO_VfxContainer container in _vfxContainers)
            {
                string name = container.VfxID;
                _accessor[name] = container.VfxPrefab;
                _toPool[name] = container.DefaultPoolCount;
            }
            Pool();
        }

        public void PlayVFX(Vector3 position, Vector3 size, string vfxName, Quaternion rotation, float time = 1)
        {
            if (_accessor.ContainsKey(vfxName))
            {
                ParticleSystem vfxinstance = PoolManager.GetInstance<ParticleSystem>(_accessor[vfxName]);
                vfxinstance.transform.position = position;
                vfxinstance.transform.localScale = size;
                vfxinstance.transform.rotation = rotation;
                StartCoroutine(KillInstance(vfxinstance, time));
            }
        }

        private void Pool()
        {
            foreach (string key in _toPool.Keys)
            {
                PoolManager.PoolGameObject(_accessor[key], "", _toPool[key]);
            }
        }

        IEnumerator KillInstance(ParticleSystem ins, float time)
        {
            yield return new WaitForSeconds(time);
            PoolManager.ReleaseInstance<ParticleSystem>(ins);
        }    
    }

   


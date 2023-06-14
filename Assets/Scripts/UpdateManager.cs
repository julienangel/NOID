using System.Collections.Generic;
using UnityEngine;

public class UpdateManager : MonoBehaviour
{
    private readonly List<IUpdater> _updaters = new List<IUpdater>();

    #region Singleton Interface

    private static UpdateManager _instance;

    public static UpdateManager Instance
    {
        get
        {
            if (_instance != null) return _instance;
            
            GameObject singletonObject = new GameObject();
            _instance = singletonObject.AddComponent<UpdateManager>();
            singletonObject.name = "UpdateManager";
            DontDestroyOnLoad(singletonObject);

            return _instance;
        }
    }
    
    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    #endregion

    public void AddBehaviour(IUpdater behaviour)
    {
        _updaters.Add(behaviour);
    }

    public void RemoveBehaviour(IUpdater behaviour)
    {
        _updaters.Remove(behaviour);
    }

    private void Update()
    {
        foreach (var updater in _updaters)
        {
            updater.UpdateNormal(Time.deltaTime);
        }
    }
}
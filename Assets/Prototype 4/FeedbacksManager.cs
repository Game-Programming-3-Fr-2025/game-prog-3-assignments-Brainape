using UnityEngine;
using MoreMountains.Feedbacks;
using System.Collections.Generic;

public class FeedbacksManager : MonoBehaviour
{
    public static FeedbacksManager Instance;

    [System.Serializable]
    public class NamedFeedback
    {
        public string id;
        public MMF_Player player;
    }

    [SerializeField] private List<NamedFeedback> feedbacks = new List<NamedFeedback>();

    private Dictionary<string, MMF_Player> _lookup;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        _lookup = new Dictionary<string, MMF_Player>();
        foreach (var f in feedbacks)
        {
            if (!_lookup.ContainsKey(f.id) && f.player != null)
                _lookup.Add(f.id, f.player);
        }
    }

    public void Play(string id, Vector3 position)
    {
        if (_lookup.TryGetValue(id, out var player))
        {
            player.transform.position = position;
            player.PlayFeedbacks();
        }
        else
        {
            Debug.LogWarning($"[FeedbacksManager] No feedback registered with id: {id}");
        }
    }

    public void Play(string id, Transform target)
    {
        if (_lookup.TryGetValue(id, out var player))
        {
            player.transform.position = target.position;
            player.PlayFeedbacks(target.position);
        }
    }
}

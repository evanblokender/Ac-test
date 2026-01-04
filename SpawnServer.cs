using System.Net;
using System.Net.Sockets;
using System.Text;
using UnityEngine;
using System.Collections.Generic;

public class SpawnServer : MonoBehaviour
{
    UdpClient udp;
    public List<GameObject> spawnableItems;

    Dictionary<string, GameObject> itemMap;

    void Start()
    {
        itemMap = new Dictionary<string, GameObject>();
        foreach (var item in spawnableItems)
            itemMap[item.name] = item;

        udp = new UdpClient(7777);
        udp.BeginReceive(OnReceive, null);
    }

    void OnReceive(System.IAsyncResult ar)
    {
        IPEndPoint ep = new IPEndPoint(IPAddress.Any, 7777);
        byte[] data = udp.EndReceive(ar, ref ep);
        udp.BeginReceive(OnReceive, null);

        string json = Encoding.UTF8.GetString(data);
        HandleMessage(json);
    }

    void HandleMessage(string json)
    {
        var msg = JsonUtility.FromJson<SpawnMessage>(json);
        if (msg.type != "spawn") return;
        if (!itemMap.ContainsKey(msg.item)) return;

        for (int i = 0; i < msg.count; i++)
        {
            Vector3 pos = Camera.main.transform.position;
            Instantiate(itemMap[msg.item], pos, Quaternion.identity);
        }
    }

    [System.Serializable]
    class SpawnMessage
    {
        public string type;
        public string item;
        public int count;
    }
}

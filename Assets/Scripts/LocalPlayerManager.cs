using UnityEngine;

public class LocalPlayerManager : MonoBehaviour
{
  public static LocalPlayerManager Instance { get; private set; }

  public Ship LocalPlayer { get; private set; }

  private void Awake()
  {
    Instance = this;
  }

  public void SetLocalPlayer(Ship player)
  {
    LocalPlayer = player;
  }
}
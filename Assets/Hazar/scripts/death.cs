using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class death : MonoBehaviour
{
  public float deathHeight = 10f; // Ölüm yüksekliği
  
      private Animator animator; // Animator bileşeni referansı
  
      private bool isDead = false; // Karakter ölü mü?
      private float deadCooldown;
  
      void Start()
      {
          animator = GetComponent<Animator>(); // Animator bileşeni referansını al
          deadCooldown = 0;
      }
  
      void Update()
      {
          Debug.Log(deadCooldown);
          deadCooldown -= Time.deltaTime;
          // Karakterin yüksekliği deathHeight değerinin altına düştüğünde ölüm animasyonunu oynat
          if (transform.position.y <= deathHeight && !isDead)
          {
              isDead = true;
              PlayDeathAnimation();
              deadCooldown = 2;

          }

          if (isDead && deadCooldown<=0)
          {
              Debug.Log("deading people");
              SceneManager.LoadScene(2);
              
          }
      }
  
      void PlayDeathAnimation()
      {
          // Ölüm animasyonunu oynatmak için animator bileşenindeki "Death" trigger'ını tetikle
          animator.SetTrigger("Death");
          // Ek olarak, karakterin hareketini veya kontrolünü devre dışı bırakabilirsiniz.
          // Örneğin, karakterin Rigidbody bileşenini veya kontrol script'ini devre dışı bırakabilirsiniz.
      }
}

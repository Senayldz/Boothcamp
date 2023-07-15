using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class death : MonoBehaviour
{
  public float deathHeight = 10f; // Ölüm yüksekliği
  
      private Animator animator; // Animator bileşeni referansı
  
      private bool isDead = false; // Karakter ölü mü?
  
      void Start()
      {
          animator = GetComponent<Animator>(); // Animator bileşeni referansını al
      }
  
      void Update()
      {
          // Karakterin yüksekliği deathHeight değerinin altına düştüğünde ölüm animasyonunu oynat
          if (transform.position.y <= deathHeight && !isDead)
          {
              isDead = true;
              PlayDeathAnimation();
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

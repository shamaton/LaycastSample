using UnityEngine;
using System.Collections;

public class Cube : MonoBehaviour {
  // 移動パラメータ
  public float addX      = 0.1f;
  public float threshold = 3f;

  // キャッシュコンポーネント
  private Transform myTrans;

  // Ray
  private Ray ray;
  private float distance = 3f;

  // layer mask
  private int layerMask;

  // 色制御用
  private Coroutine cor;

  // Use this for initialization
  void Start () {
    myTrans = gameObject.transform;
    ray = new Ray(myTrans.position, Vector3.down);

    int layerNo = LayerMask.NameToLayer("Ground");
    layerMask = 1 << layerNo;
  }

  // Update is called once per frame
  void Update () {
    Vector3 pos = myTrans.position;

    // 位置を更新
    pos.x += addX;
    myTrans.position = pos;

    // 向きを変更
    if (Mathf.Abs(pos.x) > threshold) {
      addX = -addX;
    }

    // raycast
    RaycastHit hit;
    ray.origin = myTrans.position;
    if (Physics.Raycast(ray, out hit, distance, layerMask)) {
      if (cor != null) {
        StopCoroutine(cor);
      }
      cor = StartCoroutine(red(hit.collider.gameObject));
    }

    // SceneビューにRayを表示する
    Debug.DrawRay(ray.origin, ray.direction * distance, Color.red);
  }

  private IEnumerator red(GameObject obj) {
    // 赤色
    Renderer r = obj.GetComponent<Renderer>();
    r.material.SetColor("_Color", Color.red);
    yield return new WaitForSeconds(0.1f);

    // 元に戻す
    r.material.SetColor("_Color", Color.white);
    cor = null;
  }
}

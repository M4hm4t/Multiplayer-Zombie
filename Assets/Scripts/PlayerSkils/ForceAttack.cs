using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Code;

public class ForceAttack : MonoBehaviour
{
    private SkillController skillController;
    PlayerController enemy;
    // Vector3 start = Vector3.zero;
    public Vector3 end = new Vector3(7, 7, 7);
    public float t = 0.5f;

    // Start is called before the first frame update

    public void Start()
    {
        enemy = GetComponent<PlayerController>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.CompareTag($"Enemy"))
        {
            other.GetComponent<EnemyController>().GetHit();
        }
    }
    private void Update()
    {
        ForceSkill();
    }

    public void ForceSkill()
    {
        transform.localScale = Vector3.MoveTowards(transform.localScale, end, Time.deltaTime * t);
        if (end.sqrMagnitude - transform.localScale.sqrMagnitude < 0.1)
        {
            skillController.canForce = true;
            Destroy(gameObject);
        }
    }
    public void SetSkillController(SkillController skillController)
    {

        this.skillController = skillController;
    }
}

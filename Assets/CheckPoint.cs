using UnityEngine;

public class CheckPoint : MonoBehaviour
{

    [SerializeField] private int CheckPointIndex;

    public void OnTriggerStay2D(Collider2D collision)
    {

        TrySaveCheckPoint();

    }

    private void TrySaveCheckPoint()
    {

        bool isExist = false;

        for(int i = 0;i< Metric.SceneOnloadVarible.GameScene.CurrentSide.SideRecord.checkPointRecords.Count; ++i)
        {

            if (Metric.SceneOnloadVarible.GameScene.CurrentSide.SideRecord.checkPointRecords[i].CheckPointIndex == CheckPointIndex)
            {

                isExist = true;

                break;

            }

        }

        if (!isExist)
        {

            var newRecord =
                new Metric.Archive.SaveFile.MapRecord.SideRecord.CheckPointRecord
                {
                    CheckPointIndex = CheckPointIndex,
                    Strawberries = new System.Collections.Generic.List<int>()
                };

            Metric.SceneOnloadVarible.GameScene.CurrentSide.SideRecord.checkPointRecords.Add(newRecord);

            Metric.SceneOnloadVarible.GameScene.CurrentSide.CheckPoints[CheckPointIndex].CheckPointRecord = newRecord;

        }
        
        gameObject.SetActive(false);

    }

}

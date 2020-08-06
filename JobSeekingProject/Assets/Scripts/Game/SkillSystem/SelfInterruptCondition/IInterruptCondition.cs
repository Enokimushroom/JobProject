using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IInterruptCondition
{
    bool Check(Deployer deployer);

    void OnFinish(Deployer deployer);
}

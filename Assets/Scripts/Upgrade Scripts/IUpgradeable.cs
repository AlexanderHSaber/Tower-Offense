using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//interface for classes that can receive upgrades

public interface IUpgradeable
{
    //get an upgrade from somewhere
    public void ReceiveUpgrade(BaseUpgrade upgrade);

    //do something with the upgrade
    public void ApplyUpgrade(BaseUpgrade upgrade);
}

using System;
using CoreBluetooth;
using CoreLocation;

namespace AstroBuilders.iOS
{

	public delegate void StateUpdatedEvent(CBPeripheralManager peripheral);

	public class PeripheralManagerDelegate : CBPeripheralManagerDelegate
	{
		public event StateUpdatedEvent StateUpdatedEvent;

		public override void StateUpdated (CBPeripheralManager peripheral)
		{
			if (StateUpdatedEvent != null)
				StateUpdatedEvent (peripheral);
		}

	}
}
using System;
using System.IO;
using dwd.core;
using hydra.enums;

namespace h
{
	public class O : VersionedModel
	{
		public O(N playmat)
		{
			this.phases = playmat;
		}

		public bool get_Active()
		{
			this.updateActive();
			return this.active;
		}

		private void set_Active(bool value)
		{
			if (this.active != value)
			{
				this.active = value;
				base.markDirty();
			}
		}

		private void updateActive()
		{
			if (this.active)
			{
				if (this.matchData == null)
				{
					Finder.TryFind<HydraMatchData>(out this.matchData);
				}
				this.set_Active((this.phases == null || this.phases.get_Phase() == null || this.phases.get_Phase() == Phases.StartGame) && !this.matchData.get_Options().A);
			}
		}

		public bool get_Minimized()
		{
			return this.minimized;
		}

		public void set_Minimized(bool value)
		{
			if (this.minimized != value)
			{
				this.minimized = value;
				base.markDirty();
			}
		}

		public bool get_ShowDialog()
		{
			return this.showDialog;
		}

		public void set_ShowDialog(bool value)
		{
			if (this.showDialog != value)
			{
				this.showDialog = value;
				base.markDirty();
			}
		}

		public void End()
		{
			this.set_Active(false);
			File.AppendAllText("sent.txt", "muligan ended\n");
		}

		public override ulong get_Version()
		{
			this.updateActive();
			return base.get_Version();
		}

		private readonly N phases;

		private HydraMatchData matchData;

		private bool active = true;

		private bool minimized;

		private bool showDialog;
	}
}

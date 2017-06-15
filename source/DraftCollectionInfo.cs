using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.CompilerServices;
using dwd.core.archetypes;
using dwd.core.draft.serialization;

namespace dwd.core.draft
{
	public class DraftCollectionInfo
	{
		public DraftCollectionInfo(SerializableDraftCollectionInfo serializable) : this(from twople in serializable.A
		select twople.get_Item1())
		{
		}

		public DraftCollectionInfo(IEnumerable<ArchetypeID> previousPicks)
		{
			this.A = new List<ArchetypeID>(previousPicks).AsReadOnly();
		}

		public readonly ReadOnlyCollection<ArchetypeID> A;

		[CompilerGenerated]
		private static Func<Tuple<ArchetypeID, bool>, ArchetypeID> func;
	}
}

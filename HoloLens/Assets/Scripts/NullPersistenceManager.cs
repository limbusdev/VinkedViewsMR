/*
Vinked Views
Copyright(C) 2018  Georg Eckert

This program is free software: you can redistribute it and/or modify
it under the terms of the GNU General Public License as published by
the Free Software Foundation, either version 3 of the License, or
(at your option) any later version.

This program is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
GNU General Public License for more details.

You should have received a copy of the GNU General Public License
along with this program.  If not, see <http://www.gnu.org/licenses/>.
*/

using UnityEngine;

public class NullPersistenceManager : APersistenceManager
{
    public static readonly string TAG = "NullPersistenceManager";

    public override void Load()
    {
        Debug.Log(TAG + ": Dummy. Doing nothing.");
    }

    public override void Save()
    {
        Debug.Log(TAG + ": Dummy. Doing nothing.");
    }

    public override void Initialize()
    {
        Debug.Log(TAG + ": Dummy. Doing nothing.");
    }

    public override void LoadPersistentETV(SerializedETV etv)
    {
        Debug.Log(TAG + ": Dummy. Doing nothing.");
    }

    public override void PersistETV(GameObject etv, int dataSetID, string[] variables, VisType visType)
    {
        Debug.Log(TAG + ": Dummy. Doing nothing.");
    }
}
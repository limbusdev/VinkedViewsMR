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

public interface IObserver<T>
{
    /// <summary>
    /// Observe the given observable
    /// </summary>
    /// <param name="observable"></param>
    void Observe(T observable);

    /// <summary>
    /// Unsubscribe from the given observable
    /// </summary>
    /// <param name="observable"></param>
    void Ignore(T observable);

    /// <summary>
    /// What to do, when the given observable will be destroyed
    /// </summary>
    /// <param name="observable"></param>
    void OnDispose(T observable);

    /// <summary>
    /// What to do, when the given observable has changed
    /// </summary>
    /// <param name="observable"></param>
    void OnChange(T observable);
}

/**
 * \file      IScriptEtatJouable.cs
 * \author    
 * \version   1.0
 * \date      16 décembre 2014
 * \brief	Interface héritée par les scripts activés lors du passage à un état jouable, et désactivés lors du passage à un état non jouable
 */

public interface IScriptEtatJouable {
	bool isEnabled();
	void setEnabled( bool ok );
}
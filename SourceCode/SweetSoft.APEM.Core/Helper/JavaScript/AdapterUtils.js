function CanHaveClass__BootstrapFriendlyControlAdapters(element)
{
	return ((element != null) && (element.className != null));
}

function HasAnyClass__BootstrapFriendlyControlAdapters(element)
{
	return (CanHaveClass__BootstrapFriendlyControlAdapters(element) && (element.className.length > 0));
}

function HasClass__BootstrapFriendlyControlAdapters(element, specificClass)
{
	return (HasAnyClass__BootstrapFriendlyControlAdapters(element) && (element.className.indexOf(specificClass) > -1));
}

function AddClass__BootstrapFriendlyControlAdapters(element, classToAdd)
{
	if (HasAnyClass__BootstrapFriendlyControlAdapters(element))
	{
		if (!HasClass__BootstrapFriendlyControlAdapters(element, classToAdd))
		{
			element.className = element.className + " " + classToAdd;
		}
	}
	else if (CanHaveClass__BootstrapFriendlyControlAdapters(element))
	{
		element.className = classToAdd;
	}
}

function AddClassUpward__BootstrapFriendlyControlAdapters(startElement, stopParentClass, classToAdd)
{
	var elementOrParent = startElement;
	while ((elementOrParent != null) && (!HasClass__BootstrapFriendlyControlAdapters(elementOrParent, topmostClass)))
	{
		AddClass__BootstrapFriendlyControlAdapters(elementOrParent, classToAdd);
		elementOrParent = elementOrParent.parentNode;
	}    
}

function SwapClass__BootstrapFriendlyControlAdapters(element, oldClass, newClass)
{
	if (HasAnyClass__BootstrapFriendlyControlAdapters(element))
	{
		element.className = element.className.replace(new RegExp(oldClass, "gi"), newClass);
	}
}

function SwapOrAddClass__BootstrapFriendlyControlAdapters(element, oldClass, newClass)
{
	if (HasClass__BootstrapFriendlyControlAdapters(element, oldClass))
	{
		SwapClass__BootstrapFriendlyControlAdapters(element, oldClass, newClass);
	}
	else
	{
		AddClass__BootstrapFriendlyControlAdapters(element, newClass);
	}
}

function RemoveClass__BootstrapFriendlyControlAdapters(element, classToRemove)
{
	SwapClass__BootstrapFriendlyControlAdapters(element, classToRemove, "");
}

function RemoveClassUpward__BootstrapFriendlyControlAdapters(startElement, stopParentClass, classToRemove)
{
	var elementOrParent = startElement;
	while ((elementOrParent != null) && (!HasClass__BootstrapFriendlyControlAdapters(elementOrParent, topmostClass)))
	{
		RemoveClass__BootstrapFriendlyControlAdapters(elementOrParent, classToRemove);
		elementOrParent = elementOrParent.parentNode;
	}    
}

function IsEnterKey()
{
	var retVal = false;
	var keycode = 0;
	if ((typeof(window.event) != "undefined") && (window.event != null))
	{
		keycode = window.event.keyCode;
	}
	else if ((typeof(e) != "undefined") && (e != null))
	{
		keycode = e.which;
	}
	if (keycode == 13)
	{
		retVal = true;
	}
	return retVal;
}

/* Derived from http://simonwillison.net/2004/May/26/addLoadEvent/ */
function addLoadEvent( func ) 
{
	var oldonload = window.onload;
	
	if ( typeof window.onload != 'function' ) 
	{
		window.onload = func;
	}
	else
	{
		window.onload = function() {
			if (oldonload) { oldonload(); }
			func();
		}
	}
}

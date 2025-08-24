$(function () {
    function getCookie(name) {
        const cookies = document.cookie.split(';');
        for (let i = 0; i < cookies.length; i++) {
            const cookie = cookies[i].trim();
            // Check if this cookie has the specified name
            if (cookie.startsWith(name + '=')) {
                // Return the cookie's value
                return cookie.substring(name.length + 1);
            }
        }
        // If the cookie with the specified name is not found, return null
        return null;
    }
    //$('.control-sidebar').ControlSidebar('collapse');
    //$('.control-sidebar').ControlSidebar('toggle');
    //$('.control-sidebar').ControlSidebar('show');
    //sidebar-collapse
    
    document.body.classList.toggle('sidebar-collapse');
});
/// <reference path="../../Scripts/angular.js" />
/*global todomvc */
'use strict';

/**
 * Services that persists and retrieves TODOs from localStorage
*/
todomvc.factory('todoStorage', function ($http) {
	var STORAGE_ID = 'todos-angularjs-perf';

	return {
	    get: function (callback) {
	        $http.get("api/Todo").success(function (data) {
	            callback(data);
	        });	        
		},

	    put: function (todos, callback) {
	        $http.post("api/Todo", todos).success(function(){
	            if (callback) callback();
	        });
		}
	};
});

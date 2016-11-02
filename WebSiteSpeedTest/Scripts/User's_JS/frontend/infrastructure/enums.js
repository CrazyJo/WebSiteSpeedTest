"use strict";
(function (HttpMethod) {
    HttpMethod[HttpMethod["GET"] = 0] = "GET";
    HttpMethod[HttpMethod["POST"] = 1] = "POST";
    HttpMethod[HttpMethod["PUT"] = 2] = "PUT";
    HttpMethod[HttpMethod["DELETE"] = 3] = "DELETE";
})(exports.HttpMethod || (exports.HttpMethod = {}));
var HttpMethod = exports.HttpMethod;
(function (PagerElementRole) {
    PagerElementRole[PagerElementRole["First"] = 1] = "First";
    PagerElementRole[PagerElementRole["Next"] = 2] = "Next";
    PagerElementRole[PagerElementRole["Previous"] = 3] = "Previous";
    PagerElementRole[PagerElementRole["Last"] = 4] = "Last";
})(exports.PagerElementRole || (exports.PagerElementRole = {}));
var PagerElementRole = exports.PagerElementRole;
//# sourceMappingURL=enums.js.map
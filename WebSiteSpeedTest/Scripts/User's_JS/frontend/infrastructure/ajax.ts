/// <reference path="../../../typings/jquery/jquery.d.ts" />

import * as Enums from "./enums";

export class Ajax
{
    static run<T, TDoneEventArg>(httpMethodType: Enums.HttpMethod,
        url: string,
        data: T,
        callBackOnDone: JQueryPromiseCallback<TDoneEventArg>)
    {
        $.ajax({
            type: Enums.HttpMethod[httpMethodType],
            url: url,
            data: data
        })
            .done(callBackOnDone);
    }
}
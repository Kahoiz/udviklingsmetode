<?php

use Illuminate\Support\Facades\Route;

Route::get('/{number}', function () {
    $number = request()->route('number');
    $fibonacci = function ($n) use (&$fibonacci) {
        if ($n <= 1) {
            return $n;
        }
        return $fibonacci($n - 1) + $fibonacci($n - 2);
    };

    return response()->json([
        'fibonacci' => $fibonacci($number)
    ]);
});

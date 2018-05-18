package moe.tlaster.shiba

/**
 * Created by SE on 3/5.
 */
enum class Orientation {
    Vertical,
    Horizontal
}

data class Thickness(val value: String?) {
    val top: Float
    val left: Float
    val right: Float
    val bottom: Float

    init {
        if (value == null) {
            top = 0F
            left = 0F
            right = 0F
            bottom = 0F
        } else {
            val values = value.trim('[').trim(']').split(',').map { it.trim() }
            when (values.count()) {
                1 -> {
                    val result = values[0].toFloat()
                    top = result
                    left = result
                    right = result
                    bottom = result
                }
                2 -> {
                    val vertical = values[1].toFloat()
                    val horizon = values[0].toFloat()
                    left = horizon
                    right = horizon
                    top = vertical
                    bottom = vertical
                }
                4 -> {
                    left = values[0].toFloat()
                    top = values[1].toFloat()
                    right = values[2].toFloat()
                    bottom = values[3].toFloat()
                }
                else -> {
                    top = 0F
                    left = 0F
                    right = 0F
                    bottom = 0F
                }
            }
        }
    }
}

data class Percent(val percent: String?) {
    val value: Number = if (percent == null) {
        0F
    }
    else {
        percent.trimEnd('%').toDouble() / 100F
    }

}

open class View(val viewName: String, val properties: Map<String, Any>, val children: ArrayList<View> = ArrayList()) {
}
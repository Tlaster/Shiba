package org.shibajs.shiba.mapper

import android.widget.Button
import org.shibajs.shiba.IShibaContext

class ButtonMapper :ViewMapper<Button>() {
    override fun createNativeView(context: IShibaContext): Button {
        return Button(context.getContext())
    }

    override fun propertyMaps(): ArrayList<PropertyMap> {
        return super.propertyMaps().apply {
            add(PropertyMap("text", { view, it ->
                if (view is Button && it is String) {
                    view.text = it
                }
            }))
        }
    }
}
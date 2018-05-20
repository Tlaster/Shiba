package moe.tlaster.shibasample

import moe.tlaster.shiba.BaseNotifyObjet
import moe.tlaster.shiba.Binding

class Model : BaseNotifyObjet() {


    @get:Binding(name = "androidText")
    @set:Binding(name = "androidText")
    var text = "hello world!"
        set(text) {
            field = text
            notifyPropertyChanged("androidText")
        }
}
import Foundation
import UIKit

// インスタンス化
@_cdecl("createUISelectionFeedbackGenerator")
func createUISelectionFeedbackGenerator() -> UnsafeMutableRawPointer {
    let instance : UISelectionFeedbackGenerator
    if #available(iOS 17.5, *),
       let rootView = UnityFramework.getInstance().appController().rootView {
        instance = UISelectionFeedbackGenerator(view: rootView)
    } else {
        instance = UISelectionFeedbackGenerator()
    }
    
    let unmanaged = Unmanaged<UISelectionFeedbackGenerator>.passRetained(instance)
    return unmanaged.toOpaque()
}

// 解放
@_cdecl("releaseUISelectionFeedbackGenerator")
func releaseUISelectionFeedbackGenerator(_ instancePtr: UnsafeRawPointer) {
    let unmanaged = Unmanaged<UISelectionFeedbackGenerator>.fromOpaque(instancePtr)
    unmanaged.release()
}

// 準備
@_cdecl("prepareUISelectionFeedbackGenerator")
func prepareUISelectionFeedbackGenerator(_ instancePtr: UnsafeRawPointer) {
    let instance = Unmanaged<UISelectionFeedbackGenerator>.fromOpaque(instancePtr).takeUnretainedValue()
    instance.prepare()
}

// 再生
@_cdecl("selectionChangedUISelectionFeedbackGenerator")
func selectionChangedUISelectionFeedbackGenerator(_ instancePtr: UnsafeRawPointer) {
    let instance = Unmanaged<UISelectionFeedbackGenerator>.fromOpaque(instancePtr).takeUnretainedValue()
    instance.selectionChanged()
}

import Foundation
import UIKit

// インスタンス化
@_cdecl("init_UINotificationFeedbackGenerator")
func init_UINotificationFeedbackGenerator() -> UnsafeMutableRawPointer {
    let instance: UINotificationFeedbackGenerator
    if #available(iOS 17.5, *),
       let rootView = UnityFramework.getInstance().appController().rootView {
        instance = UINotificationFeedbackGenerator(view: rootView)
    } else {
        instance = UINotificationFeedbackGenerator()
    }
    
    let unmanaged = Unmanaged<UINotificationFeedbackGenerator>.passRetained(instance)
    return unmanaged.toOpaque()
}

// 解放
@_cdecl("release_UINotificationFeedbackGenerator")
func release_UINotificationFeedbackGenerator(_ instancePtr: UnsafeRawPointer) {
    let unmanaged = Unmanaged<UINotificationFeedbackGenerator>.fromOpaque(instancePtr)
    unmanaged.release()
}

// 準備
@_cdecl("prepare_UINotificationFeedbackGenerator")
func prepare_UINotificationFeedbackGenerator(_ instancePtr: UnsafeRawPointer) {
    let instance = Unmanaged<UINotificationFeedbackGenerator>.fromOpaque(instancePtr).takeUnretainedValue()
    instance.prepare()
}

// 再生
@_cdecl("notificationOccurred_UINotificationFeedbackGenerator")
func notificationOccurred_UINotificationFeedbackGenerator(_ instancePtr: UnsafeRawPointer, _ notificationType: Int32) {
    guard let notificationType = UINotificationFeedbackGenerator.FeedbackType(rawValue: Int(notificationType)) else {
        fatalError("invalid type")
    }
    
    let instance = Unmanaged<UINotificationFeedbackGenerator>.fromOpaque(instancePtr).takeUnretainedValue()
    instance.notificationOccurred(notificationType)
}

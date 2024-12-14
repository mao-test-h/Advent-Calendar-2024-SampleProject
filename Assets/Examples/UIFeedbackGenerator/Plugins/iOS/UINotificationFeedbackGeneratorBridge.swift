import Foundation
import UIKit

// インスタンス化
@_cdecl("createUINotificationFeedbackGenerator")
func createUINotificationFeedbackGenerator() -> UnsafeMutableRawPointer {
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
@_cdecl("releaseUINotificationFeedbackGenerator")
func releaseUINotificationFeedbackGenerator(_ instancePtr: UnsafeRawPointer) {
    let unmanaged = Unmanaged<UINotificationFeedbackGenerator>.fromOpaque(instancePtr)
    unmanaged.release()
}

// 準備
@_cdecl("prepareUINotificationFeedbackGenerator")
func prepareUINotificationFeedbackGenerator(_ instancePtr: UnsafeRawPointer) {
    let instance = Unmanaged<UINotificationFeedbackGenerator>.fromOpaque(instancePtr).takeUnretainedValue()
    instance.prepare()
}

// 再生
@_cdecl("notificationOccurredUINotificationFeedbackGenerator")
func notificationOccurredUINotificationFeedbackGenerator(_ instancePtr: UnsafeRawPointer, _ notificationType: Int32) {
    guard let notificationType = UINotificationFeedbackGenerator.FeedbackType(rawValue: Int(notificationType)) else {
        fatalError("invalid type")
    }
    
    let instance = Unmanaged<UINotificationFeedbackGenerator>.fromOpaque(instancePtr).takeUnretainedValue()
    instance.notificationOccurred(notificationType)
}

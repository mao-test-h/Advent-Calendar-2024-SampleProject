public typealias CCharPtr = UnsafeMutablePointer<CChar>?

public typealias OnErrorDelegate = @convention(c) (CCharPtr) -> Void
public typealias OnStoppedDelegate = @convention(c) (Int32) -> Void
public typealias OnResetDelegate = @convention(c) () -> Void

// MARK: - Extensions

extension String {
    // String を CChar のポインタに変換
    func toCharPtr() -> CCharPtr {
        let utfText = (self as NSString).utf8String!;
        let pointer = UnsafeMutablePointer<Int8>.allocate(capacity: (8 * self.count) + 1);
        return UnsafeMutablePointer(strcpy(pointer, utfText))
    }
}

extension CCharPtr {
    // CChar のポインタを String に変換
    func toString() -> String {
        return String(cString: self!);
    }
}
